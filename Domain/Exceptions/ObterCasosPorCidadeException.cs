using System;
namespace Domain.Exceptions
{
	public class GetCasesByCityException : BaseException
	{
		public GetCasesByCityException(string message) : base(message)
		{
		}
	}
}

