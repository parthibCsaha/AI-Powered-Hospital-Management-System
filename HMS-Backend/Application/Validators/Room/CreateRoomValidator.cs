

using FluentValidation;
using HMS_Backend.Application.DTOs.Room;

namespace HMS_Backend.Application.Validators.Room;


public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.RoomNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.RoomType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Floor).GreaterThanOrEqualTo(0);
        RuleFor(x => x.DailyRate).GreaterThan(0);
    }
}