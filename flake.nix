{
  inputs = {
    utils.url = "github:numtide/flake-utils";
  };
  outputs =
    {
      self,
      nixpkgs,
      utils,
    }:
    utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = nixpkgs.legacyPackages.${system};
      in
      {
        devShell = pkgs.mkShell {
          buildInputs = with pkgs; [
            dotnet-ef
            (
              with dotnetCorePackages;
              combinePackages [
                sdk_8_0
                sdk_9_0
              ]
            )
          ];
        };
      }
    );
}
