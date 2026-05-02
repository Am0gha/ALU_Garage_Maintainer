export interface ICar{
    class: string;
    rarity: string;
    maxStars: number;
    maxRank: number;
    name: string;
    fuel: number;
    topSpeed: number;
    acceleration: number;
    handling: number;
    nitro: number;
    hasEips: boolean;
    noEips: number | null;
    requiresKey: boolean;
    bp1sCount: number|null;
    bp2sCount: number|null;
    bp3sCount: number|null;
    bp4sCount: number|null;
    bp5sCount: number|null;
    bp6sCount: number|null;
}