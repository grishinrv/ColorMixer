using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorMixer.Storage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColorNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Red = table.Column<byte>(type: "INTEGER", nullable: false),
                    Green = table.Column<byte>(type: "INTEGER", nullable: false),
                    Blue = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorNodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MixingSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixingSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mixings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RightColorId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeftColorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Operation = table.Column<byte>(type: "INTEGER", nullable: false),
                    SetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mixings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mixings_ColorNodes_LeftColorId",
                        column: x => x.LeftColorId,
                        principalTable: "ColorNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mixings_ColorNodes_RightColorId",
                        column: x => x.RightColorId,
                        principalTable: "ColorNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mixings_MixingSets_SetId",
                        column: x => x.SetId,
                        principalTable: "MixingSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mixings_LeftColorId",
                table: "Mixings",
                column: "LeftColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixings_RightColorId",
                table: "Mixings",
                column: "RightColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Mixings_SetId",
                table: "Mixings",
                column: "SetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mixings");

            migrationBuilder.DropTable(
                name: "ColorNodes");

            migrationBuilder.DropTable(
                name: "MixingSets");
        }
    }
}
