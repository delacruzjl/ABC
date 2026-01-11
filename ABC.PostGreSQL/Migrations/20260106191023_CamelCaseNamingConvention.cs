using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.PostGreSQL.Migrations
{
    /// <inheritdoc />
    public partial class CamelCaseNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AntecedentObservation_Antecedents_AntecedentsId",
                table: "AntecedentObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_AntecedentObservation_Observations_ObservationsId",
                table: "AntecedentObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorObservation_Behaviors_BehaviorsId",
                table: "BehaviorObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_BehaviorObservation_Observations_ObservationsId",
                table: "BehaviorObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildChildCondition_ChildConditions_ConditionsId",
                table: "ChildChildCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_ChildChildCondition_Children_childrenId",
                table: "ChildChildCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsequenceObservation_Consequences_ConsequencesId",
                table: "ConsequenceObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsequenceObservation_Observations_ObservationsId",
                table: "ConsequenceObservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Observations",
                table: "Observations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consequences",
                table: "Consequences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConsequenceObservation",
                table: "ConsequenceObservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Children",
                table: "Children");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChildConditions",
                table: "ChildConditions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChildChildCondition",
                table: "ChildChildCondition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Behaviors",
                table: "Behaviors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BehaviorObservation",
                table: "BehaviorObservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Antecedents",
                table: "Antecedents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AntecedentObservation",
                table: "AntecedentObservation");

            migrationBuilder.RenameTable(
                name: "Observations",
                newName: "observations");

            migrationBuilder.RenameTable(
                name: "Consequences",
                newName: "consequences");

            migrationBuilder.RenameTable(
                name: "ConsequenceObservation",
                newName: "consequenceObservation");

            migrationBuilder.RenameTable(
                name: "Children",
                newName: "children");

            migrationBuilder.RenameTable(
                name: "ChildConditions",
                newName: "childConditions");

            migrationBuilder.RenameTable(
                name: "ChildChildCondition",
                newName: "childChildCondition");

            migrationBuilder.RenameTable(
                name: "Behaviors",
                newName: "behaviors");

            migrationBuilder.RenameTable(
                name: "BehaviorObservation",
                newName: "behaviorObservation");

            migrationBuilder.RenameTable(
                name: "Antecedents",
                newName: "antecedents");

            migrationBuilder.RenameTable(
                name: "AntecedentObservation",
                newName: "antecedentObservation");

            migrationBuilder.RenameColumn(
                name: "When",
                table: "observations",
                newName: "when");

            migrationBuilder.RenameColumn(
                name: "Version",
                table: "observations",
                newName: "version");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "observations",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "observations",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "observations",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "observations",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "observations",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "ChildId",
                table: "observations",
                newName: "childId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "observations",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Observations_ChildId",
                table: "observations",
                newName: "iX_observations_childId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "consequences",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "consequences",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "consequences",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "consequences",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "consequences",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "consequences",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ObservationsId",
                table: "consequenceObservation",
                newName: "observationsId");

            migrationBuilder.RenameColumn(
                name: "ConsequencesId",
                table: "consequenceObservation",
                newName: "consequencesId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsequenceObservation_ObservationsId",
                table: "consequenceObservation",
                newName: "iX_consequenceObservation_observationsId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "children",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "children",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "children",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "children",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "children",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "BirthYear",
                table: "children",
                newName: "birthYear");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "children",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "childConditions",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "childConditions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "childConditions",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "childConditions",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "childConditions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ConditionsId",
                table: "childChildCondition",
                newName: "conditionsId");

            migrationBuilder.RenameIndex(
                name: "IX_ChildChildCondition_childrenId",
                table: "childChildCondition",
                newName: "iX_childChildCondition_childrenId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "behaviors",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "behaviors",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "behaviors",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "behaviors",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "behaviors",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "behaviors",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ObservationsId",
                table: "behaviorObservation",
                newName: "observationsId");

            migrationBuilder.RenameColumn(
                name: "BehaviorsId",
                table: "behaviorObservation",
                newName: "behaviorsId");

            migrationBuilder.RenameIndex(
                name: "IX_BehaviorObservation_ObservationsId",
                table: "behaviorObservation",
                newName: "iX_behaviorObservation_observationsId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "antecedents",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "antecedents",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "antecedents",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "antecedents",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "antecedents",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "antecedents",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ObservationsId",
                table: "antecedentObservation",
                newName: "observationsId");

            migrationBuilder.RenameColumn(
                name: "AntecedentsId",
                table: "antecedentObservation",
                newName: "antecedentsId");

            migrationBuilder.RenameIndex(
                name: "IX_AntecedentObservation_ObservationsId",
                table: "antecedentObservation",
                newName: "iX_antecedentObservation_observationsId");

            migrationBuilder.AddPrimaryKey(
                name: "pK_observations",
                table: "observations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_consequences",
                table: "consequences",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_consequenceObservation",
                table: "consequenceObservation",
                columns: new[] { "consequencesId", "observationsId" });

            migrationBuilder.AddPrimaryKey(
                name: "pK_children",
                table: "children",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_childConditions",
                table: "childConditions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_childChildCondition",
                table: "childChildCondition",
                columns: new[] { "conditionsId", "childrenId" });

            migrationBuilder.AddPrimaryKey(
                name: "pK_behaviors",
                table: "behaviors",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_behaviorObservation",
                table: "behaviorObservation",
                columns: new[] { "behaviorsId", "observationsId" });

            migrationBuilder.AddPrimaryKey(
                name: "pK_antecedents",
                table: "antecedents",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_antecedentObservation",
                table: "antecedentObservation",
                columns: new[] { "antecedentsId", "observationsId" });

            migrationBuilder.AddForeignKey(
                name: "fK_antecedentObservation_antecedents_antecedentsId",
                table: "antecedentObservation",
                column: "antecedentsId",
                principalTable: "antecedents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_antecedentObservation_observations_observationsId",
                table: "antecedentObservation",
                column: "observationsId",
                principalTable: "observations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_behaviorObservation_behaviors_behaviorsId",
                table: "behaviorObservation",
                column: "behaviorsId",
                principalTable: "behaviors",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_behaviorObservation_observations_observationsId",
                table: "behaviorObservation",
                column: "observationsId",
                principalTable: "observations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_childChildCondition_childConditions_conditionsId",
                table: "childChildCondition",
                column: "conditionsId",
                principalTable: "childConditions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_childChildCondition_children_childrenId",
                table: "childChildCondition",
                column: "childrenId",
                principalTable: "children",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_consequenceObservation_consequences_consequencesId",
                table: "consequenceObservation",
                column: "consequencesId",
                principalTable: "consequences",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_consequenceObservation_observations_observationsId",
                table: "consequenceObservation",
                column: "observationsId",
                principalTable: "observations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_observations_children_childId",
                table: "observations",
                column: "childId",
                principalTable: "children",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_antecedentObservation_antecedents_antecedentsId",
                table: "antecedentObservation");

            migrationBuilder.DropForeignKey(
                name: "fK_antecedentObservation_observations_observationsId",
                table: "antecedentObservation");

            migrationBuilder.DropForeignKey(
                name: "fK_behaviorObservation_behaviors_behaviorsId",
                table: "behaviorObservation");

            migrationBuilder.DropForeignKey(
                name: "fK_behaviorObservation_observations_observationsId",
                table: "behaviorObservation");

            migrationBuilder.DropForeignKey(
                name: "fK_childChildCondition_childConditions_conditionsId",
                table: "childChildCondition");

            migrationBuilder.DropForeignKey(
                name: "fK_childChildCondition_children_childrenId",
                table: "childChildCondition");

            migrationBuilder.DropForeignKey(
                name: "fK_consequenceObservation_consequences_consequencesId",
                table: "consequenceObservation");

            migrationBuilder.DropForeignKey(
                name: "fK_consequenceObservation_observations_observationsId",
                table: "consequenceObservation");

            migrationBuilder.DropForeignKey(
                name: "fK_observations_children_childId",
                table: "observations");

            migrationBuilder.DropPrimaryKey(
                name: "pK_observations",
                table: "observations");

            migrationBuilder.DropPrimaryKey(
                name: "pK_consequences",
                table: "consequences");

            migrationBuilder.DropPrimaryKey(
                name: "pK_consequenceObservation",
                table: "consequenceObservation");

            migrationBuilder.DropPrimaryKey(
                name: "pK_children",
                table: "children");

            migrationBuilder.DropPrimaryKey(
                name: "pK_childConditions",
                table: "childConditions");

            migrationBuilder.DropPrimaryKey(
                name: "pK_childChildCondition",
                table: "childChildCondition");

            migrationBuilder.DropPrimaryKey(
                name: "pK_behaviors",
                table: "behaviors");

            migrationBuilder.DropPrimaryKey(
                name: "pK_behaviorObservation",
                table: "behaviorObservation");

            migrationBuilder.DropPrimaryKey(
                name: "pK_antecedents",
                table: "antecedents");

            migrationBuilder.DropPrimaryKey(
                name: "pK_antecedentObservation",
                table: "antecedentObservation");

            migrationBuilder.RenameTable(
                name: "observations",
                newName: "Observations");

            migrationBuilder.RenameTable(
                name: "consequences",
                newName: "Consequences");

            migrationBuilder.RenameTable(
                name: "consequenceObservation",
                newName: "ConsequenceObservation");

            migrationBuilder.RenameTable(
                name: "children",
                newName: "Children");

            migrationBuilder.RenameTable(
                name: "childConditions",
                newName: "ChildConditions");

            migrationBuilder.RenameTable(
                name: "childChildCondition",
                newName: "ChildChildCondition");

            migrationBuilder.RenameTable(
                name: "behaviors",
                newName: "Behaviors");

            migrationBuilder.RenameTable(
                name: "behaviorObservation",
                newName: "BehaviorObservation");

            migrationBuilder.RenameTable(
                name: "antecedents",
                newName: "Antecedents");

            migrationBuilder.RenameTable(
                name: "antecedentObservation",
                newName: "AntecedentObservation");

            migrationBuilder.RenameColumn(
                name: "when",
                table: "Observations",
                newName: "When");

            migrationBuilder.RenameColumn(
                name: "version",
                table: "Observations",
                newName: "Version");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Observations",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Observations",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Observations",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Observations",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Observations",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "childId",
                table: "Observations",
                newName: "ChildId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Observations",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "iX_observations_childId",
                table: "Observations",
                newName: "IX_Observations_ChildId");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Consequences",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Consequences",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Consequences",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Consequences",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Consequences",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Consequences",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "observationsId",
                table: "ConsequenceObservation",
                newName: "ObservationsId");

            migrationBuilder.RenameColumn(
                name: "consequencesId",
                table: "ConsequenceObservation",
                newName: "ConsequencesId");

            migrationBuilder.RenameIndex(
                name: "iX_consequenceObservation_observationsId",
                table: "ConsequenceObservation",
                newName: "IX_ConsequenceObservation_ObservationsId");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Children",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "Children",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "Children",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Children",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Children",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "birthYear",
                table: "Children",
                newName: "BirthYear");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Children",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "ChildConditions",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "ChildConditions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "ChildConditions",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "ChildConditions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ChildConditions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "conditionsId",
                table: "ChildChildCondition",
                newName: "ConditionsId");

            migrationBuilder.RenameIndex(
                name: "iX_childChildCondition_childrenId",
                table: "ChildChildCondition",
                newName: "IX_ChildChildCondition_childrenId");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Behaviors",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Behaviors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Behaviors",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Behaviors",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Behaviors",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Behaviors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "observationsId",
                table: "BehaviorObservation",
                newName: "ObservationsId");

            migrationBuilder.RenameColumn(
                name: "behaviorsId",
                table: "BehaviorObservation",
                newName: "BehaviorsId");

            migrationBuilder.RenameIndex(
                name: "iX_behaviorObservation_observationsId",
                table: "BehaviorObservation",
                newName: "IX_BehaviorObservation_ObservationsId");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Antecedents",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Antecedents",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Antecedents",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Antecedents",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Antecedents",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Antecedents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "observationsId",
                table: "AntecedentObservation",
                newName: "ObservationsId");

            migrationBuilder.RenameColumn(
                name: "antecedentsId",
                table: "AntecedentObservation",
                newName: "AntecedentsId");

            migrationBuilder.RenameIndex(
                name: "iX_antecedentObservation_observationsId",
                table: "AntecedentObservation",
                newName: "IX_AntecedentObservation_ObservationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Observations",
                table: "Observations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consequences",
                table: "Consequences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConsequenceObservation",
                table: "ConsequenceObservation",
                columns: new[] { "ConsequencesId", "ObservationsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Children",
                table: "Children",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChildConditions",
                table: "ChildConditions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChildChildCondition",
                table: "ChildChildCondition",
                columns: new[] { "ConditionsId", "childrenId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Behaviors",
                table: "Behaviors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BehaviorObservation",
                table: "BehaviorObservation",
                columns: new[] { "BehaviorsId", "ObservationsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Antecedents",
                table: "Antecedents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AntecedentObservation",
                table: "AntecedentObservation",
                columns: new[] { "AntecedentsId", "ObservationsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AntecedentObservation_Antecedents_AntecedentsId",
                table: "AntecedentObservation",
                column: "AntecedentsId",
                principalTable: "Antecedents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AntecedentObservation_Observations_ObservationsId",
                table: "AntecedentObservation",
                column: "ObservationsId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorObservation_Behaviors_BehaviorsId",
                table: "BehaviorObservation",
                column: "BehaviorsId",
                principalTable: "Behaviors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehaviorObservation_Observations_ObservationsId",
                table: "BehaviorObservation",
                column: "ObservationsId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildChildCondition_ChildConditions_ConditionsId",
                table: "ChildChildCondition",
                column: "ConditionsId",
                principalTable: "ChildConditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChildChildCondition_Children_childrenId",
                table: "ChildChildCondition",
                column: "childrenId",
                principalTable: "Children",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsequenceObservation_Consequences_ConsequencesId",
                table: "ConsequenceObservation",
                column: "ConsequencesId",
                principalTable: "Consequences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsequenceObservation_Observations_ObservationsId",
                table: "ConsequenceObservation",
                column: "ObservationsId",
                principalTable: "Observations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Observations_Children_ChildId",
                table: "Observations",
                column: "ChildId",
                principalTable: "Children",
                principalColumn: "Id");
        }
    }
}
