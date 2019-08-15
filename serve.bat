if not exist "Build" mkdir Build
if not exist "Build\AutoUpdater" mkdir Build\AutoUpdater
if not exist "Build\Squirrel" mkdir Build\Squirrel
if not exist "Build\NetSparkle" mkdir Build\NetSparkle

cd Build

python -m http.server 8000
