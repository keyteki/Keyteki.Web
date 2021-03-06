﻿namespace Keyteki.Web.Services
{
    using CrimsonDev.Gameteki.Api.Services;
    using CrimsonDev.Gameteki.Data;
    using CrimsonDev.Gameteki.Data.Models.Config;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class KeytekiUserService : UserService
    {
        public KeytekiUserService(
            IGametekiDbContext context,
            IOptions<AuthTokenOptions> optionsAccessor,
            IOptions<GametekiApiOptions> lobbyOptions,
            IEmailSender emailSender,
            IViewRenderService viewRenderService,
            ILogger<UserService> logger,
            IStringLocalizer<UserService> localizer)
            : base(context, optionsAccessor, lobbyOptions, emailSender, viewRenderService, logger, localizer)
        {
        }
    }
}
