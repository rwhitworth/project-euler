#include <stdio.h>
#include "mini-gmp.h"

int is_prime(mpz_t number)
{
	char* result_str;

	result_str = mpz_get_str(NULL, 10, number);
	mpz_t zero;
	mpz_t two;
	mpz_t three;
	mpz_t five;
	mpz_init_set_si(zero, 0);
	mpz_init_set_si(two, 2);
	mpz_init_set_si(three, 3);
	mpz_init_set_si(five, 5);

	// speed up small integers
	if (mpz_fits_slong_p(number))
	{
		int number_i = mpz_get_si(number);
		if (number_i < 2)
		{
			return 0;
		}
		if (number_i == 2)
		{
			return 1;
		} 
		if (number_i % 2 == 0)
		{
			return 0;
		}
		if (number_i == 3)
		{
			return 1;
		}
		if (number_i % 3 == 0)
		{
			return 0;
		}
		if (number_i == 5)
		{
			return 1;
		}
		if (number_i % 5 == 0)
		{
			return 0;
		}
	}

	mpz_t sqrt;
	mpz_init(sqrt);
	mpz_sqrt(sqrt, number);
	mpz_add_ui(sqrt, sqrt, 1);
	mpz_t counter;
	mpz_init_set_ui(counter, 3);
	mpz_t mod;
	mpz_init(mod);
	char* counter_s;
	char* sqrt_s;
	char* mod_s;
	counter_s = mpz_get_str(NULL, 10, counter);
	sqrt_s = mpz_get_str(NULL, 10, sqrt);
	mod_s = mpz_get_str(NULL, 10, mod);


	//for (counter; mpz_cmp(counter, sqrt) <= 0; mpz_add_ui(counter, counter, two))
	while (1 == 1)
	{
		counter_s = mpz_get_str(NULL, 10, counter);
		sqrt_s = mpz_get_str(NULL, 10, sqrt);
		mpz_mod(mod, number, counter);
		mod_s = mpz_get_str(NULL, 10, mod);
		if (mpz_cmp(mod, zero) == 0)
		{
			return 0;
		}
		mpz_add(counter, counter, two);
		if (mpz_cmp(counter, sqrt) > 0)
		{
			break;
		}
	}

	return 1;
}

void main()
{
	mpz_t prime;
	mpz_init_set_str(prime, "600851475143", 10);
	mpz_t counter;
	mpz_init_set_ui(counter, 3);
	mpz_t highest;
	mpz_init_set_ui(highest, 0);
	mpz_t sqrt;
	mpz_init_set_ui(sqrt, 0);
	mpz_t zero;
	mpz_init_set_ui(zero, 0);
	mpz_t one;
	mpz_init_set_ui(one, 1);
	mpz_t mod;
	mpz_init_set_ui(mod, 0);

	mpz_sqrt(sqrt, prime);

	while (mpz_cmp(sqrt, counter) >= 0)
	{
		mpz_mod(mod, prime, counter);

		if (mpz_cmp(mod, zero) == 0)
		{
			if (is_prime(counter))
			{
				mpz_set(highest, counter);
			}
		}
		mpz_add(counter, counter, one);
		mpz_add(counter, counter, one);
	}

	printf("%s\n", mpz_get_str(NULL, 10, highest));
}