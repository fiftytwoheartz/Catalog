using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Repo {

    public static class TryCatchHelper {

        public static async Task<ResponseBuilder> TryCatchAsync(
            Func<Task<ResponseBuilder>> tryLogic,
            Dictionary<Type, Func<Exception, string>> toBeCaughtExceptions = null,
			Func<Exception, string> errorMessageBuilder = null)
        {
	        try {
                return await tryLogic();
            }
	        catch (Exception e) when ((toBeCaughtExceptions ?? (toBeCaughtExceptions =
		                                       new Dictionary<Type, Func<Exception, string>>
			                                       {
				                                       {
					                                       e.GetType(),
					                                       errorMessageBuilder ?? (exception => exception.Message)
				                                       }
			                                       })).ContainsKey(e.GetType()))
            {
                return toBeCaughtExceptions[e.GetType()](e).Failure();
            }
        }

    }

}