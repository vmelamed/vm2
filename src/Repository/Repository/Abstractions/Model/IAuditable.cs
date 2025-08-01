﻿namespace vm2.Repository.Abstractions.Model;

/// <summary>
/// Domain objects that implement <c>IAuditable</c> store auditing information that can be populated automatically by the
/// repository and can be used to track the lifecycle of the entity. The entities that implement this interface can also be
/// soft-deleted, meaning that they are not physically removed from the database but marked as deleted.
/// </summary>
/// <remarks>
/// See also <seealso cref="ISoftDeletable"/> for auditing soft deletion.<br/>
/// </remarks>
public interface IAuditable
{
    /// <summary>
    /// Gets or sets the date and time when the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    string CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the entity was last updated.
    /// </summary>
    DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last updated the entity.
    /// </summary>
    string UpdatedBy { get; set; }

    /// <summary>
    /// Sets all properties of an added <see cref="IAuditable"/> with current values.
    /// </summary>
    /// <param name="now">
    /// The timestamp to record as the added and updated time. If <see langword="null"/>, the current UTC time is used.
    /// </param>
    /// <param name="actor">
    /// The identifier of the actor performing the addition. Can be an empty string if not specified.
    /// </param>
    void AuditOnAdd(
        DateTimeOffset? now = default,
        string actor = "")
    {
        CreatedAt = now ?? DateTimeOffset.UtcNow;
        CreatedBy = actor;
        AuditOnUpdate(now, actor);
    }

    /// <summary>
    /// Sets all properties of an updated <see cref="IAuditable"/> with current values.
    /// </summary>
    /// <param name="now">
    /// </param>
    /// <param name="actor">
    /// The identifier of the actor performing the update. Can be an empty string if not specified.
    /// </param>
    void AuditOnUpdate(
        DateTimeOffset? now = default,
        string actor = "")
    {
        UpdatedAt = now ?? DateTimeOffset.UtcNow;
        UpdatedBy = actor;
    }
}
