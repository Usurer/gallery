using Core.Abstractions;

namespace Api.BackgroundServices
{
    public class ScheduledScanService : BackgroundService
    {
        private readonly IServiceProvider Services;
        private Timer? timer = null;
        private bool IsRunning;

        public ScheduledScanService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            timer = new Timer(GetItemsAsync, null, 10 * 1000, 5 * 1000);
            return;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async void GetItemsAsync(object? state)
        {
            if (IsRunning)
                return;

            IsRunning = true;
            using var scope = Services.CreateScope();
            var storageService = scope.ServiceProvider.GetRequiredService<IStorageService>();
            var fileSystemService = scope.ServiceProvider.GetRequiredService<IFileSystemService>();

            var item = await storageService.GetScanTarget();
            if (item.HasValue)
            {
                // TODO: We do not return the result of this execution, so pushing it to memory with the ToArrayAsync() isn't good.
                // Probably I can rewrite the ScanFoldersFromRootAsync itself, to create a version that puts data to DB, but doesn't return it
                await fileSystemService.ScanFoldersFromRootAsync(item.Value.path).ToArrayAsync();
            }

            IsRunning = false;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}