using System;
using System.Linq;
using System.Threading.Tasks;

using Catalog.Models;

namespace Catalog.WebApi {

    public static class TryCatchHelper {

        public static async Task<ResponseBuilder> TryCatchAsync(
            Func<Task<ResponseBuilder>> tryLogic,
            Func<Exception, string> errorMessageBuilder = null,
                Type[] toBeCaughtExceptions = null) {
            try {
                return await tryLogic();
            } catch (Exception e) when ((toBeCaughtExceptions ?? new[] { e.GetType() }).Contains(e.GetType())) {
                var buildedErrorMessage = (errorMessageBuilder ?? (exception => exception.Message))(e);
                return buildedErrorMessage.Failure();
            }
        }

    }

}