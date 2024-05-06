﻿using SlowlySimulate.Domain.Constants;

namespace SlowlySimulate.Domain.Models;
public class UserLanguage : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; set; }
    public int LanguageId { get; set; }
    public LanguageLevel LanguageLevel { get; set; }
}