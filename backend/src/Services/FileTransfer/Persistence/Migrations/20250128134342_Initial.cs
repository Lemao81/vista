using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "filetransfer");

            migrationBuilder.CreateTable(
                name: "media_folders",
                schema: "filetransfer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    storage_version = table.Column<byte>(type: "smallint", nullable: false),
                    original_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_folders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "media_items",
                schema: "filetransfer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_folder_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_kind = table.Column<int>(type: "integer", nullable: false),
                    media_size_kind = table.Column<int>(type: "integer", nullable: false),
                    storage_version = table.Column<byte>(type: "smallint", nullable: false),
                    meta_data = table.Column<string>(type: "text", nullable: false),
                    created_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    modified_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_media_items_media_folders_media_folder_id",
                        column: x => x.media_folder_id,
                        principalSchema: "filetransfer",
                        principalTable: "media_folders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_media_items_media_folder_id",
                schema: "filetransfer",
                table: "media_items",
                column: "media_folder_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_items",
                schema: "filetransfer");

            migrationBuilder.DropTable(
                name: "media_folders",
                schema: "filetransfer");
        }
    }
}
