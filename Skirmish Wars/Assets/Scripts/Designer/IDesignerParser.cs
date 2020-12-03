/// <summary>
/// Defines the requirements for a parser
/// from a stage designer.
/// </summary>
public interface IDesignerParser
{
    /// <summary>
    /// Retrieves the first tile grid from the designer.
    /// </summary>
    /// <returns>The tile grid with initialized properties.</returns>
    TileGrid GetFirstTileGrid();
    /// <summary>
    /// Retrieves the first damage table from the designer.
    /// </summary>
    /// <returns>The initialized damage table.</returns>
    DamageTable GetFirstDamageTable();
    /// <summary>
    /// Retrieves all preplaced actors from the designer.
    /// </summary>
    /// <param name="onGrid">The grid context that actors will be added to.</param>
    /// <returns>An array of all tile actors with initialized properties.</returns>
    TileActor[] GetAllPreplacedActors(TileGrid onGrid);
    /// <summary>
    /// Retrieves all preplaced commanders from the designer.
    /// </summary>
    /// <param name="onGrid">The grid context that commanders will be added to.</param>
    /// <returns>An array of all commanders with initialized properties.</returns>
    Commander[] GetAllPreplacedCommanders(TileGrid onGrid);
}
