namespace Ets2Client.Telemetry.Data
{
    // copied from https://github.com/nlhans/ets2-sdk-plugin/blob/2e0b68553d6d6181ccf7be99f1a512d63fc4d231/ets2-client/C%23/Ets2SdkClient/Ets2SdkBoolean.cs
    // todo: validate and check for changes
    public enum Ets2SdkBoolean
    {
        CruiseControl,
        Wipers,
        ParkBrake,
        MotorBrake,
        ElectricEnabled,
        EngineEnabled,

        BlinkerLeftActive,
        BlinkerRightActive,
        BlinkerLeftOn,
        BlinkerRightOn,

        LightsParking,
        LightsBeamLow,
        LightsBeamHigh,
        LightsAuxFront,
        LightsAuxRoof,
        LightsBeacon,
        LightsBrake,
        LightsReverse,

        BatteryVoltageWarning,
        AirPressureWarning,
        AirPressureEmergency,
        AdblueWarning,
        OilPressureWarning,
        WaterTemperatureWarning,
        TrailerAttached
    }
}