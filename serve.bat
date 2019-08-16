md ^
    Build\Squirrel\NetFx^
    Build\Squirrel\Core^
    Build\AutoUpdater^
    Build\NetSparkle^
    2> NUL

cd Build

python -m http.server 8000
