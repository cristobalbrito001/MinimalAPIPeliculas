﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalAPIPeliculas.Migrations
{
    /// <inheritdoc />
    public partial class actorPelicula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActoresPeliculas",
                columns: table => new
                {
                    ActorID = table.Column<int>(type: "int", nullable: false),
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Personaje = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActoresPeliculas", x => new { x.PeliculaId, x.ActorID });
                    table.ForeignKey(
                        name: "FK_ActoresPeliculas_Actores_ActorID",
                        column: x => x.ActorID,
                        principalTable: "Actores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActoresPeliculas_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActoresPeliculas_ActorID",
                table: "ActoresPeliculas",
                column: "ActorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActoresPeliculas");
        }
    }
}
