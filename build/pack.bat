set version=0.2.0
dotnet pack ../src/LazyData -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/LazyData.Numerics -c Release -o ../../_dist /p:version=%version%
dotnet pack ../src/LazyData.SuperLazy -c Release -o ../../_dist /p:version=%version%