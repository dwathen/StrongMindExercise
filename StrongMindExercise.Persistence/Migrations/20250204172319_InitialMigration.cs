﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StrongMindExercise.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Pizzas",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Pizzas", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Toppings",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Toppings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PizzaTopping",
            columns: table => new
            {
                PizzasId = table.Column<int>(type: "INTEGER", nullable: false),
                ToppingsId = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PizzaTopping", x => new { x.PizzasId, x.ToppingsId });
                table.ForeignKey(
                    name: "FK_PizzaTopping_Pizzas_PizzasId",
                    column: x => x.PizzasId,
                    principalTable: "Pizzas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PizzaTopping_Toppings_ToppingsId",
                    column: x => x.ToppingsId,
                    principalTable: "Toppings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PizzaTopping_ToppingsId",
            table: "PizzaTopping",
            column: "ToppingsId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PizzaTopping");

        migrationBuilder.DropTable(
            name: "Pizzas");

        migrationBuilder.DropTable(
            name: "Toppings");
    }
}
