namespace CoursesAPI.Services.Utilities
{
	public class DateTimeUtils
	{
		public static bool IsLeapYear(int year)
		{
			//Standard rules should be used: a year is a leap year if it is 
			//divisible by 4,unless it is divisible by 100 
			//(except when it is divisible by 400).
			
			if ((year % 4 == 0 && year % 100 != 0)||year % 400 == 0){
				return true;
			}
			
			return false;
		}
	}
}
