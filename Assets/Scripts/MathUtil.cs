using System;

/*
 * Author: James Moessis
 * This class is for random calculations that there was no immediate 
 * library/function for. 
 */
public static class MathUtil
{
	public static float[] LinSpace(float start, float end, int divisions) {
        float[] output = new float[divisions];
        float increment = (end - start) / divisions;
        float currentValue = start;
        for(int i = 0; i < divisions; i++) {
            output[i] = currentValue;
            currentValue += increment;
        }
        return output;
    }
}
