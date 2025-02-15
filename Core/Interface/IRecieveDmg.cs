namespace TBoGV;
internal interface IRecieveDmg
{
	float Hp { get; set; }
	int MaxHp { get; set; }
	void RecieveDmg(float damage);
}