using HMS_Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMS_Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260419090000_AddAppointmentOverlapExclusionConstraint")]
    public partial class AddAppointmentOverlapExclusionConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS btree_gist;");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Appointments""
                ADD CONSTRAINT ""EX_Appointments_Doctor_TimeRange_NoOverlap""
                EXCLUDE USING gist (
                    ""DoctorId"" WITH =,
                    tsrange(
                        (""AppointmentDate"" AT TIME ZONE 'UTC') + ""StartTime"",
                        (""AppointmentDate"" AT TIME ZONE 'UTC') + ""EndTime"",
                        '[)'
                    ) WITH &&
                )
                WHERE (""Status"" <> 'Cancelled' AND NOT ""IsDeleted"");
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE ""Appointments""
                DROP CONSTRAINT IF EXISTS ""EX_Appointments_Doctor_TimeRange_NoOverlap"";
            ");
        }
    }
}
