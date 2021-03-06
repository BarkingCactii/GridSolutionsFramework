//******************************************************************************************************
//  TransportTypes.cpp - Gbtc
//
//  Copyright � 2018, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  03/23/2018 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

#include "TransportTypes.h"
#include "../Common/Convert.h"

using namespace std;
using namespace GSF;
using namespace GSF::TimeSeries;

SubscriberException::SubscriberException(string message) noexcept :
    m_message(move(message))
{
}

const char* SubscriberException::what() const noexcept
{
    return &m_message[0];
}

PublisherException::PublisherException(string message) noexcept :
    m_message(move(message))
{
}

const char* PublisherException::what() const noexcept
{
    return &m_message[0];
}

Measurement::Measurement() :
    ID(0),
    SignalID(Empty::Guid),
    Value(NAN),
    Adder(0),
    Multiplier(1),
    Timestamp(0),
    Flags(0)
{
}

float64_t Measurement::AdjustedValue() const
{
    return Value * Multiplier + Adder;
}

DateTime Measurement::GetDateTime() const
{
    return FromTicks(Timestamp);
}

void Measurement::GetUnixTime(time_t& unixSOC, uint16_t& milliseconds) const
{
    ToUnixTime(Timestamp, unixSOC, milliseconds);
}

SignalReference::SignalReference() :
    SignalID(Empty::Guid),
    Index(0),
    Kind(SignalKind::Unknown)
{    
}

SignalReference::SignalReference(const string& signal) : SignalID(Guid())
{
    // Signal reference may contain multiple dashes, we're interested in the last one
    const auto splitIndex = signal.find_last_of('-');

    // Assign default values to fields
    Index = 0;

    if (splitIndex == string::npos)
    {
        // This represents an error - best we can do is assume entire string is the acronym
        Acronym = ToUpper(Trim(signal));
        Kind = SignalKind::Unknown;
    }
    else
    {
        string signalType = ToUpper(Trim(signal.substr(splitIndex + 1)));
        Acronym = ToUpper(Trim(signal.substr(0, splitIndex)));

        // If the length of the signal type acronym is greater than 2, then this
        // is an indexed signal type (e.g., CORDOVA-PA2)
        if (signalType.length() > 2)
        {
            Kind = ParseSignalKind(signalType.substr(0, 2));

            if (Kind != SignalKind::Unknown)
                Index = stoi(signalType.substr(2));
        }
        else
        {
            Kind = ParseSignalKind(signalType);
        }
    }
}

const char* GSF::TimeSeries::SignalKindDescription[] =
{
    "Angle",
    "Magnitude",
    "Frequency",
    "DfDt",
    "Status",
    "Digital",
    "Analog",
    "Calculation",
    "Statistic",
    "Alarm",
    "Quality",
    "Unknown"
};

const char* GSF::TimeSeries::SignalKindAcronym[] =
{
    "PA",
    "PM",
    "FQ",
    "DF",
    "SF",
    "DV",
    "AV",
    "CV",
    "ST",
    "AL",
    "QF",
    "??"
};

// SignalReference.ToString()
ostream& GSF::TimeSeries::operator << (ostream& stream, const SignalReference& reference)
{
    if (reference.Index > 0)
        return stream << reference.Acronym << "-" << SignalKindAcronym[reference.Kind] << reference.Index;

    return stream << reference.Acronym << "-" << SignalKindAcronym[reference.Kind];
}

string GSF::TimeSeries::GetSignalTypeAcronym(SignalKind kind, char phasorType)
{
    switch (kind)
    {
        case Angle:
            return toupper(phasorType) == 'V' ? "VPHA" : "IPHA";
        case Magnitude:
            return toupper(phasorType) == 'V' ? "VPHM" : "IPHM";
        case Frequency:
            return "FREQ";
        case DfDt:
            return "DFDT";
        case Status:
            return "FLAG";
        case Digital:
            return "DIGI";
        case Analog:
            return "ALOG";
        case Calculation:
            return "CALC";
        case Statistic:
            return "STAT";
        case Alarm:
            return "ALRM";
        case Quality:
            return "QUAL";
        case Unknown:
        default:
            return "NULL";
    }
}

std::string TimeSeries::GetEngineeringUnits(const std::string& signalType)
{
    if (IsEqual(signalType, "IPHM"))
        return "Amps";

    if (IsEqual(signalType, "VPHM"))
        return "Volts";

    if (IsEqual(signalType, "FREQ"))
        return "Hz";

    if (EndsWith(signalType, "PHA"))
        return "Degrees";

    return Empty::String;
}

std::string GSF::TimeSeries::GetProtocolType(const std::string& protocolName)
{
    if (StartsWith(protocolName, "Gateway") ||
        StartsWith(protocolName, "Modbus") ||
        StartsWith(protocolName, "DNP"))
            return "Measurement";

    return "Frame";
}

void TimeSeries::ParseMeasurementKey(const std::string& key, std::string& source, uint32_t& id)
{
    const vector<string> parts = Split(key, ":");

    if (parts.size() == 2)
    {
        source = parts[0];
        id = uint32_t(stoul(parts[1]));
    }
    else
    {
        source = parts[0];
        id = UInt32::MaxValue;
    }
}

// Gets the "SignalKind" enum for the specified "acronym".
//  params:
//	   acronym: Acronym of the desired "SignalKind"
//  returns: The "SignalKind" for the specified "acronym".
SignalKind GSF::TimeSeries::ParseSignalKind(const string& acronym)
{
    if (acronym == "PA") // Phase Angle
        return SignalKind::Angle;

    if (acronym == "PM") // Phase Magnitude
        return SignalKind::Magnitude;

    if (acronym == "FQ") // Frequency
        return SignalKind::Frequency;

    if (acronym == "DF") // dF/dt
        return SignalKind::DfDt;

    if (acronym == "SF") // Status Flags
        return SignalKind::Status;

    if (acronym == "DV") // Digital Value
        return SignalKind::Digital;

    if (acronym == "AV") // Analog Value
        return SignalKind::Analog;

    if (acronym == "CV") // Calculated Value
        return SignalKind::Calculation;

    if (acronym == "ST") // Statistical Value
        return SignalKind::Statistic;

    if (acronym == "AL") // Alarm Value
        return SignalKind::Alarm;

    if (acronym == "QF") // Quality Flags
        return SignalKind::Quality;

    return SignalKind::Unknown;
}