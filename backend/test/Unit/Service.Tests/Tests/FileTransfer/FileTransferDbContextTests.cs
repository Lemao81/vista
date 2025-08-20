using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Service.Tests.Utilities;

namespace Service.Tests.Tests.FileTransfer;

public class FileTransferDbContextTests
{
	[Fact]
	public async Task GenerateScript_Should_GenerateProperSchema()
	{
		// Arrange
		await using var fixture = new FileTransferDbContextFixture();

		// Act
		var script = fixture.DbContext.Database.GenerateCreateScript();

		// Assert
		await Verify(script);
	}

	[Fact]
	public async Task Migrations_Should_BeUpToDate()
	{
		// Arrange
		await using var fixture = new FileTransferDbContextFixture();

		var migrationModelDiffer    = fixture.DbContext.GetService<IMigrationsModelDiffer>();
		var migrationsAssembly      = fixture.DbContext.GetService<IMigrationsAssembly>();
		var modelRuntimeInitializer = fixture.DbContext.GetService<IModelRuntimeInitializer>();
		var designTimeModel         = fixture.DbContext.GetService<IDesignTimeModel>();

		var currentModel = designTimeModel.Model;

		var snapshotModel = migrationsAssembly.ModelSnapshot?.Model;
		if (snapshotModel is IMutableModel mutableModel)
		{
			snapshotModel = mutableModel.FinalizeModel();
		}

		if (snapshotModel is not null)
		{
			snapshotModel = modelRuntimeInitializer.Initialize(snapshotModel);
		}

		// Act
		var modelDifferences = migrationModelDiffer.GetDifferences(source: snapshotModel?.GetRelationalModel(), target: currentModel.GetRelationalModel());

		// Assert
		Assert.Empty(modelDifferences);
	}
}
