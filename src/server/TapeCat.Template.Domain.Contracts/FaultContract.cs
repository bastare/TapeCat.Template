namespace TapeCat.Template.Domain.Contracts;

using System.ComponentModel.DataAnnotations;

public sealed record FaultContract([Required] Exception Exception);