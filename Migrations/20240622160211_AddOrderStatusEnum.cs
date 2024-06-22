using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebDelivery2.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "orders_customer_id_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "orders_deliveryaddress_id_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "orders_pickupaddress_id_fkey",
                table: "orders");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status.order_status", "pending,intransit,delivered")
                .OldAnnotation("Npgsql:Enum:order_status", "pending,intransit,delivered");

            migrationBuilder.AlterColumn<int>(
                name: "pickupaddress_id",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "deliveryaddress_id",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "orders",
                type: "order_status",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "orders_customer_id_fkey",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "orders_deliveryaddress_id_fkey",
                table: "orders",
                column: "deliveryaddress_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "orders_pickupaddress_id_fkey",
                table: "orders",
                column: "pickupaddress_id",
                principalTable: "addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "orders_customer_id_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "orders_deliveryaddress_id_fkey",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "orders_pickupaddress_id_fkey",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "status",
                table: "orders");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:order_status", "pending,intransit,delivered")
                .OldAnnotation("Npgsql:Enum:order_status.order_status", "pending,intransit,delivered");

            migrationBuilder.AlterColumn<int>(
                name: "pickupaddress_id",
                table: "orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "deliveryaddress_id",
                table: "orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "orders_customer_id_fkey",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "orders_deliveryaddress_id_fkey",
                table: "orders",
                column: "deliveryaddress_id",
                principalTable: "addresses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "orders_pickupaddress_id_fkey",
                table: "orders",
                column: "pickupaddress_id",
                principalTable: "addresses",
                principalColumn: "id");
        }
    }
}
