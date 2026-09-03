using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Migrations
{
    /// <inheritdoc />
    public partial class fixGloablQueryFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetModels_AssetBrandId_ModelName",
                table: "AssetModels");

            migrationBuilder.DropIndex(
                name: "IX_AssetCategories_CategoryName",
                table: "AssetCategories");

            migrationBuilder.DropIndex(
                name: "IX_AssetBrands_AssetCategoryId_BrandName",
                table: "AssetBrands");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModels_AssetBrandId_ModelName",
                table: "AssetModels",
                columns: new[] { "AssetBrandId", "ModelName" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AssetCategories_CategoryName",
                table: "AssetCategories",
                column: "CategoryName",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_AssetBrands_AssetCategoryId_BrandName",
                table: "AssetBrands",
                columns: new[] { "AssetCategoryId", "BrandName" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AssetModels_AssetBrandId_ModelName",
                table: "AssetModels");

            migrationBuilder.DropIndex(
                name: "IX_AssetCategories_CategoryName",
                table: "AssetCategories");

            migrationBuilder.DropIndex(
                name: "IX_AssetBrands_AssetCategoryId_BrandName",
                table: "AssetBrands");

            migrationBuilder.CreateIndex(
                name: "IX_AssetModels_AssetBrandId_ModelName",
                table: "AssetModels",
                columns: new[] { "AssetBrandId", "ModelName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetCategories_CategoryName",
                table: "AssetCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetBrands_AssetCategoryId_BrandName",
                table: "AssetBrands",
                columns: new[] { "AssetCategoryId", "BrandName" },
                unique: true);
        }
    }
}
