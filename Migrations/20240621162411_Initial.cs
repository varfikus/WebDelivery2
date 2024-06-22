using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebDelivery2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status", "pending,intransit,delivered");

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    money = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValueSql: "0.00")
                },
                constraints: table =>
                {
                    table.PrimaryKey("customers_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    money = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false, defaultValueSql: "0.00"),
                    licensenumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    rating = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("drivers_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    house = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("addresses_pkey", x => x.id);
                    table.ForeignKey(
                        name: "addresses_customer_id_fkey",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: true),
                    driver_id = table.Column<int>(type: "integer", nullable: true),
                    deliveryaddress_id = table.Column<int>(type: "integer", nullable: true),
                    pickupaddress_id = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("orders_pkey", x => x.id);
                    table.ForeignKey(
                        name: "orders_customer_id_fkey",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "orders_deliveryaddress_id_fkey",
                        column: x => x.deliveryaddress_id,
                        principalTable: "addresses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "orders_driver_id_fkey",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "orders_pickupaddress_id_fkey",
                        column: x => x.pickupaddress_id,
                        principalTable: "addresses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_customer_id",
                table: "addresses",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "customers_email_key",
                table: "customers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "drivers_email_key",
                table: "drivers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "drivers_licensenumber_key",
                table: "drivers",
                column: "licensenumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_deliveryaddress_id",
                table: "orders",
                column: "deliveryaddress_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_driver_id",
                table: "orders",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_pickupaddress_id",
                table: "orders",
                column: "pickupaddress_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
