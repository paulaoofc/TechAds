# ACCEPTANCE_STEP_3.md

The Application layer implements CQRS with MediatR, featuring commands (CreateListing, UpdateListing, SubmitApplication, AuthenticateUser), queries (GetListingById, GetListings, GetUserListings), DTOs for data transfer, FluentValidation for input validation, and infrastructure interfaces (IAuthService, IEmailService, IUnitOfWork). Handlers depend solely on domain interfaces, and unit tests cover validators and command handlers using Moq.
