#include <stdio.h>
#include <stdint.h>

void main()
{
	uint64_t count = 1;
	for (int i = 0; i < 20; i++)
	{
		count *= (2 * 20) - i;
		count = count / (i + 1);
	}
	printf("%lld\n", count);
}