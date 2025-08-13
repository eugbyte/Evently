using Evently.Server.Common.Domains.Entities;
using FluentValidation;

namespace Evently.Server.Features.Members.Services;

public sealed class MemberValidator : AbstractValidator<Member> {
	public MemberValidator() {
		RuleFor((member) => member.Name).NotEmpty().WithMessage("Name is required.");
		RuleFor((member) => member.Email).NotEmpty().WithMessage("Email is required.");
		RuleForEach((member) => member.Bookings).Custom((value, context) => {
			if (value.MemberId == 0) {
				context.AddFailure("MemberId is required.");
			}

			if (string.IsNullOrEmpty(value.BookingId)) {
				context.AddFailure("BookingId is required.");
			}
		});
	}
}