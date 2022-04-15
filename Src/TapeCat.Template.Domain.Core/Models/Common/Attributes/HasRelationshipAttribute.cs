namespace TapeCat.Template.Domain.Core.Models.Common.Attributes;

// TODO: Finish me
[AttributeUsage ( AttributeTargets.Property , AllowMultiple = false )]
public sealed class HasRelationshipAttribute : Attribute
{
	public RelationshipCase Relationship { get; }

	public string PropFrom { get; }

	public string PropTo { get; }

	public bool HasCascadeBehavior { get; }

	public HasRelationshipAttribute (
		string? relationship ,
		string? propFrom ,
		string? propTo ,
		bool hasCascadeBehavior = default )
	{
		NotNullOrEmpty ( relationship );
		NotNullOrEmpty ( propFrom );
		NotNullOrEmpty ( propTo );

		PropFrom = propFrom!;
		PropTo = propTo!;
		PropTo = propTo!;
		HasCascadeBehavior = hasCascadeBehavior;
		Relationship = RelationshipCaseParse ( relationship! );

		static RelationshipCase RelationshipCaseParse ( string relationship )
		{
			if ( Enum.TryParse<RelationshipCase> ( relationship , out var relationshipCase ) )
				return relationshipCase;

			throw new ArgumentException (
				message: $"No '{relationship}' relationship. Allowed only two options: '{RelationshipCase.OneToMany}' or '{RelationshipCase.ManyToOne}'" ,
				paramName: nameof ( relationship ) );
		}
	}

	public enum RelationshipCase
	{
		OneToMany,
		ManyToOne
	}
}