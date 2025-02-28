namespace TBoGV;
interface IRecieveDmg
{
	float Hp { get; set; }
	int MaxHp { get; set; }
	float RecieveDmg(Projectile projectile);
}