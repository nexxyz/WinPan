using System;
using System.Drawing;

public class BalanceCalculator {
    public double CalculatePanningValue(int position, int maxHorizontalPixels, int minScreenX)
    {
        // Calculate the horizontal position in percent of maximum horizontal screen size
        double horizontalPositionPercent = (double)position/(double)maxHorizontalPixels;

        // Normalize the position to get the panning value
        double panningValue = Math.Max(-1.0, Math.Min(1.0, (horizontalPositionPercent * 2) - 1));

        return panningValue;
    }
}