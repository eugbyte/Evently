using Evently.Server.Common.Domains.Entities;
using FluentValidation;

namespace Evently.Server.Features.Members.Services;

public class MemberValidator : AbstractValidator<Member> {
	public MemberValidator() {
		RuleFor((attendee) => attendee.Name).NotEmpty().WithMessage("Name is required.");
		RuleFor((attendee) => attendee.Email).NotEmpty().WithMessage("Email is required.");
		RuleForEach((attendee) => attendee.BookingEvents).Custom((value, context) => {
			if (value.MemberId == 0) {
				context.AddFailure("AttendeeId is required.");
			}

			if (string.IsNullOrEmpty(value.BookingId)) {
				context.AddFailure("BookingEventId is required.");
			}
		});
		RuleForEach((attendee) => attendee.MemberCategoryDetails).Custom((value, context) => {
			if (value.MemberId == 0) {
				context.AddFailure("AttendeeId is required.");
			}

			if (value.CategoryId == 0) {
				context.AddFailure("TopicId is required.");
			}
		});
	}
}