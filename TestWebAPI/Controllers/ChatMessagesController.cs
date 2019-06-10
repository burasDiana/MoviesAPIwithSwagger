using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using TestWebAPI.Filters;
using TestWebAPI.Models.ExampleModels;
using TestWebAPI.Models.ResponseModels;
using TestWebAPI.Security;

namespace TestWebAPI.Controllers
{
    /// <summary>
    /// Handles CRUD operations on chat messages between teacher and customers
    /// </summary>
    [RoutePrefix("api/users/chatmessages")]
    public class ChatMessagesController : ApiController
    {
        /// <summary>
        /// Get all chat messages related to current user
        /// </summary>
        [Route("")]
        [CustomAuthentication(UserSecurity.UserType.Admin, UserSecurity.UserType.Customer)]
        [SwaggerResponse(HttpStatusCode.OK, "Returns all chat messages for current user", typeof(ChatMessageModelExample))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No chat messages found")]
        public IHttpActionResult Get()
        {
            //add search filter for GetAppChatMessages
            var filter = new AppChatMessageSearchFilter
            {
                CustomerId = AuthHelper.CurrentUser.Id,
            };

            if (AuthHelper.CurrentUser.Type == UserSecurity.UserType.Admin)
            {
                filter.CustomerId = null;
                filter.AdminId = AuthHelper.CurrentUser.Id;
            }

            //TODO implement get message from db like so:
            // var message = AppChatManager.GetAppChatMessages(filter);
            var query = new List<ChatMessageModel>();

            if (!query.Any())
            {
                return NotFound();
            }

            var result = (from m in query
                              //where m.SenderID == AuthHelper.CurrentUser.Id || m.ReceiverID == AuthHelper.CurrentUser.Id
                          select new ChatMessageModel
                          {
                              Id = m.Id,
                              SenderId = m.SenderId,
                              ReceiverId = m.ReceiverId,
                              DateCreated = m.DateCreated,
                              DateRead = m.DateRead,
                              Message = m.Message
                          }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get all chat messages between current user and related users
        /// </summary>
        [Route("testchats")]
        [CustomAuthentication(UserSecurity.UserType.Admin, UserSecurity.UserType.Customer)]
        [SwaggerResponse(HttpStatusCode.OK, "Returns all chat messages for current user", typeof(ChatMessageModelExample))]
        [SwaggerResponse(HttpStatusCode.NotFound, "No chat messages found")]
        [SwaggerResponse(HttpStatusCode.NotFound, "No related users found")]
        public IHttpActionResult GetTestChats(int? count = 40)
        {
            var filter = new AppChatMessageSearchFilter();
            //add search filter for GetAppChatMessages
            if (AuthHelper.CurrentUser.Type == UserSecurity.UserType.Customer)
            {
                filter.CustomerId = AuthHelper.CurrentUser.Id;
            }
            if (AuthHelper.CurrentUser.Type == UserSecurity.UserType.Admin)
            {
                filter.AdminId = AuthHelper.CurrentUser.Id;

            }
            var relatedUserIds = GetRelatedUserIds(AuthHelper.CurrentUser.Id, AuthHelper.CurrentUser.Type);

            if (!relatedUserIds.Any())
            {
                return Content(HttpStatusCode.NotFound, "No related users found");
            }

            //TODO implement get message from db like so:
            // var message = AppChatManager.GetAppChatMessages();
            var query = new List<ChatMessageModel> ();

            if (!query.Any())
            {
                return NotFound();
            }

            var result = new List<ChatMessageResponseModel>();

            foreach (var id in relatedUserIds)
            {
                var messageList = (from m in query
                                   where (m.SenderId == AuthHelper.CurrentUser.Id && m.ReceiverId == id || m.SenderId == id && m.ReceiverId == AuthHelper.CurrentUser.Id)
                                   select new ChatMessageModel
                                   {
                                       Id = m.Id,
                                       SenderId = m.SenderId,
                                       ReceiverId = m.ReceiverId,
                                       DateCreated = m.DateCreated,
                                       DateRead = m.DateRead,
                                       Message = m.Message
                                   }).ToList();
                result.Add(new ChatMessageResponseModel
                {
                    RelatedUserId = id,
                    Messages = messageList
                });
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a chat message by id
        /// </summary>
        [Route("{id}")]
        [CustomAuthentication(UserSecurity.UserType.Admin, UserSecurity.UserType.Customer)]
        [SwaggerResponse(HttpStatusCode.NotFound, "No chat message found")]
        [SwaggerResponse(HttpStatusCode.OK, "Returns one message", typeof(ChatMessageModelExample))]
        public IHttpActionResult GetbyId(int id)
        {
            //TODO implement get message from db like so:
            // var message = AppChatManager.GetAppChatMessage(id, AuthHelper.CurrentUser.Id);
            var message = new ChatMessageModel();

            if (message == null)
            {
                return NotFound();
            }
            var result = new ChatMessageModel
            {
                Id = message.Id,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                DateCreated = message.DateCreated,
                DateRead = message.DateRead,
                Message = message.Message
            };

            return Ok(result);
        }

        /// <summary>
        /// Create a new chat message
        /// </summary>
        [Route("create")]
        [ValidateModelState]
        [CustomAuthentication(UserSecurity.UserType.Admin, UserSecurity.UserType.Customer)]
        [SwaggerResponse(HttpStatusCode.OK, "Created")]
        [SwaggerResponse(HttpStatusCode.PreconditionFailed, "AppChatMessageCreated. Push notification not sent.")]
        [SwaggerRequestExample(typeof(ChatMessageCreate), typeof(ChatMessageCreateExample))]
        [SwaggerResponse(HttpStatusCode.Unauthorized, "Cannot send a message to this receiver.")]
        public IHttpActionResult Post(ChatMessageCreate request)
        {
            // check if receiver is associated with sender, otherwise deny request
            var tpSearchFilter = new AdminCustomerRelationshipFilter
            {
                AdminId = AuthHelper.CurrentUser.Type == UserSecurity.UserType.Admin ? AuthHelper.CurrentUser.Id : request.ReceiverId,
                CustomerId = AuthHelper.CurrentUser.Type == UserSecurity.UserType.Customer ? AuthHelper.CurrentUser.Id : request.ReceiverId
            };

            if (!true) //TODO implement this => TeachingPlans.FindTeachingPlansAsIQueryable(tpSearchFilter).Any()
            {
                return Content(HttpStatusCode.Unauthorized, "Permission denied.");
            }

            //TODO create chat message in database
            //var message = new ChatMessageCreate()
            //{
            //    Message = request.Message,
            //    ReceiverID = request.ReceiverId,
            //    SenderID = AuthHelper.CurrentUser.Id,
            //};
            // db.save(message)

            string deviceToken = UserSecurity.GetDeviceTokenByUserId(request.ReceiverId, out var success);

            if (!success)
            {
                return Content(HttpStatusCode.PreconditionFailed, "AppChatMessageCreated. Push notification not sent.");
            }

            string platform = "bbbbb"; //TODO implement this => Authentication.GetPlatformByDeviceToken(deviceToken);

            if (platform == null)
            {
                return Content(HttpStatusCode.PreconditionFailed, "AppChatMessageCreated. Push notification not sent.");
            }
            if (CreatePushNotification(request.Message, platform, deviceToken))
            {
                return Ok("Created. Push notification sent!");
            }

            return Content(HttpStatusCode.PreconditionFailed, "AppChatMessageCreated. Push notification not sent.");
        }

        /// <summary>
        /// Send a test push notification to current user. ONLY FOR TESTING
        /// </summary>
        [Route("testnotifications")]
        [CustomAuthentication(UserSecurity.UserType.Admin, UserSecurity.UserType.Customer)]
        public IHttpActionResult PostPushNotifications(ChatMessageCreate request)
        {
            var deviceToken = UserSecurity.GetDeviceTokenByUserId(AuthHelper.CurrentUser.Id, out var success1); //Authentication.GetDeviceTokenBySessionToken(AuthHelper.TokenValue);
            var platform = "bbbbb"; //TODO implement this => Authentication.GetPlatformBySessionToken(AuthHelper.TokenValue);
            PushNotifcationsManager.SendPushNotificationToMobileDevice(deviceToken, request.Message, platform, out var success);
            if (success)
            {
                return Ok("Success");
            }
            return Content(HttpStatusCode.BadRequest, "Push notification not sent.");
        }
        /// <summary>
        /// Edit a previously sent chat message
        /// </summary>
        public IHttpActionResult Patch(ChatMessageCreate request)
        {
            return Ok("Updated");
        }

        /// <summary>
        /// Creates a push notification to a device
        /// </summary>
        private bool CreatePushNotification(string message, string platform, string deviceToken)
        {
            PushNotifcationsManager.SendPushNotificationToMobileDevice(deviceToken, message, platform, out var success);
            return success;
        }

        /// <summary>
        /// Get the id of all related users
        /// </summary>
        /// <param name="userId"></param>
        private List<int> GetRelatedUserIds(int userId, UserSecurity.UserType userType)
        {
            return new List<int>{123,122};
            #region before
            //var response = new List<int>();
            //var studentFilter = new StudentSearchFilter();
            //if (userType == UserSecurity.UserType.Teacher)
            //{
            //    studentFilter.TeacherID = userId;

            //    var students = Students.FindStudents(studentFilter, new StudentLoadOptions
            //        { LoadTeachingPlans = true, LoadCustomer = true });

            //    if (students == null)
            //    {
            //        return new List<int>();
            //    }

            //    foreach (var student in students)
            //    {
            //        if (!response.Contains(student.CustomerID))
            //        {
            //            response.Add(student.CustomerID);
            //        }
            //    }
            //}

            //if (userType == UserSecurity.UserType.Customer)
            //{
            //    studentFilter.CustomerID = userId;

            //    var students = Students.FindStudents(studentFilter, new StudentLoadOptions
            //        { LoadTeachingPlans = true, LoadCustomer = true });

            //    if (students == null)
            //    {
            //        return response;
            //    }
            //    foreach (var student in students)
            //    {
            //        foreach (var tp in student.TeachingPlans)
            //        {
            //            if (tp.TeacherID.HasValue && !response.Contains(tp.TeacherID.Value))
            //            {
            //                response.Add(tp.TeacherID.Value);
            //            }
            //        }
            //    }
            //}

            //return response;
            #endregion
        }
    }

}