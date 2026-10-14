public static class SeatCount
{
    public const int MINIMUM = 1;
    
    public const int MAXIMUM = 6;


    public static bool WithinRange(int seats)
    {
        if (seats >= MINIMUM && seats <= MAXIMUM)
        {
            return true;
        }

        return false;
    }
}