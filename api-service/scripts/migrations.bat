cd ..
dotnet ef migrations bundle -p Database -s Api -o Api/run_migrations.exe
cd Api
run_migrations.exe

rm run_migrations.exe