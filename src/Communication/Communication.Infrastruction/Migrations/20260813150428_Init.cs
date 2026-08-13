using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Communication.Infrastruction.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Community",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyType = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Community", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicType = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommunityDiscussion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityDiscussion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityDiscussion_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunityName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostType = table.Column<int>(type: "int", nullable: false),
                    PublicType = table.Column<int>(type: "int", nullable: false),
                    Restrictions = table.Column<int>(type: "int", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityPost_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityUser_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InviteToCommunity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    ToAppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InviteToCommunity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InviteToCommunity_Community_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPostComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserPostId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPostComment_UserPost_UserPostId",
                        column: x => x.UserPostId,
                        principalTable: "UserPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPostDislike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPostId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostDislike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPostDislike_UserPost_UserPostId",
                        column: x => x.UserPostId,
                        principalTable: "UserPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPostLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPostId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPostLike_UserPost_UserPostId",
                        column: x => x.UserPostId,
                        principalTable: "UserPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityDiscussionComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommunityDiscussionId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityDiscussionComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityDiscussionComment_CommunityDiscussion_CommunityDiscussionId",
                        column: x => x.CommunityDiscussionId,
                        principalTable: "CommunityDiscussion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityPostComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommentType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    CommunityPostId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityPostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityPostComment_CommunityPost_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "CommunityPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityPostDislike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    CommunityPostId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityPostDislike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityPostDislike_CommunityPost_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "CommunityPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityPostLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    CommunityPostId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityPostLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityPostLike_CommunityPost_CommunityPostId",
                        column: x => x.CommunityPostId,
                        principalTable: "CommunityPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDiscussion_CommunityId",
                table: "CommunityDiscussion",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityDiscussionComment_CommunityDiscussionId",
                table: "CommunityDiscussionComment",
                column: "CommunityDiscussionId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPost_CommunityId",
                table: "CommunityPost",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostComment_CommunityPostId",
                table: "CommunityPostComment",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostDislike_CommunityPostId",
                table: "CommunityPostDislike",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPostLike_CommunityPostId",
                table: "CommunityPostLike",
                column: "CommunityPostId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityUser_CommunityId",
                table: "CommunityUser",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_InviteToCommunity_CommunityId",
                table: "InviteToCommunity",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostComment_UserPostId",
                table: "UserPostComment",
                column: "UserPostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostDislike_UserPostId",
                table: "UserPostDislike",
                column: "UserPostId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPostLike_UserPostId",
                table: "UserPostLike",
                column: "UserPostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityDiscussionComment");

            migrationBuilder.DropTable(
                name: "CommunityPostComment");

            migrationBuilder.DropTable(
                name: "CommunityPostDislike");

            migrationBuilder.DropTable(
                name: "CommunityPostLike");

            migrationBuilder.DropTable(
                name: "CommunityUser");

            migrationBuilder.DropTable(
                name: "InviteToCommunity");

            migrationBuilder.DropTable(
                name: "UserPostComment");

            migrationBuilder.DropTable(
                name: "UserPostDislike");

            migrationBuilder.DropTable(
                name: "UserPostLike");

            migrationBuilder.DropTable(
                name: "CommunityDiscussion");

            migrationBuilder.DropTable(
                name: "CommunityPost");

            migrationBuilder.DropTable(
                name: "UserPost");

            migrationBuilder.DropTable(
                name: "Community");
        }
    }
}
