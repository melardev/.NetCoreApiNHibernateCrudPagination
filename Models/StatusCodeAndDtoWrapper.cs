using System.Collections.Generic;
using ApiCoreNHibernateCrudPagination.Dtos.Responses.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiCoreNHibernateCrudPagination.Models

{
    public class StatusCodeAndDtoWrapper : ObjectResult
    {
        public StatusCodeAndDtoWrapper(AppResponse dto, int statusCode = 200) : base(dto)
        {
            StatusCode = statusCode;
        }

        private StatusCodeAndDtoWrapper(AppResponse dto, int statusCode, string message) : base(dto)
        {
            StatusCode = statusCode;
            if (dto.FullMessages == null)
                dto.FullMessages = new List<string>(1);
            dto.FullMessages.Add(message);
        }

        private StatusCodeAndDtoWrapper(AppResponse dto, int statusCode, ICollection<string> messages) : base(dto)
        {
            StatusCode = statusCode;
            dto.FullMessages = messages;
        }

        public static IActionResult BuildGenericNotFound()
        {
            return new StatusCodeAndDtoWrapper(new ErrorDtoResponse("Not Found"), 404);
        }

        public static StatusCodeAndDtoWrapper BuilBadRequest(ModelStateDictionary modelStateDictionary)
        {
            var errorRes = new ErrorDtoResponse();

            foreach (var key in modelStateDictionary.Keys)
            foreach (var error in modelStateDictionary[key].Errors)
                errorRes.FullMessages.Add(error.ErrorMessage);

            return new StatusCodeAndDtoWrapper(errorRes, 400);
        }

        public static IActionResult BuildSuccess(AppResponse dto)
        {
            return new StatusCodeAndDtoWrapper(dto);
        }

        public static IActionResult BuildSuccess(AppResponse dto, string message)
        {
            return new StatusCodeAndDtoWrapper(dto, 200, message);
        }


        public static IActionResult BuildSuccess(string message)
        {
            return new StatusCodeAndDtoWrapper(new SuccessResponse(message));
        }

        public static IActionResult BuildErrorResponse(string message)
        {
            return new StatusCodeAndDtoWrapper(new ErrorDtoResponse(message), 500);
        }

        public static IActionResult BuildGeneric(AppResponse dto, ICollection<string> messages = null,
            int statusCode = 200)
        {
            return new StatusCodeAndDtoWrapper(dto, statusCode, messages);
        }


        public static IActionResult BuildUnauthorized(ICollection<string> errors = null)
        {
            var res = new ErrorDtoResponse();
            if (errors != null)
                foreach (var error in errors)
                    res.FullMessages.Add(error);

            return new StatusCodeAndDtoWrapper(res, 401);
        }

        public static IActionResult BuildUnauthorized(string message = null)
        {
            if (message != null)
            {
                var fullMessages = new List<string>(1);
                fullMessages.Add(message);
                return BuildUnauthorized(fullMessages);
            }

            return BuildUnauthorized((ICollection<string>) null);
        }

        public static IActionResult BuildNotFound(AppResponse responseDto)
        {
            return new StatusCodeAndDtoWrapper(responseDto, 404);
        }
    }
}