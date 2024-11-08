using TSchedule.Persistence.Enums;

namespace TSchedule.Persistence.Models;

public record WizardResult(WizardResultType Type, string Message);
