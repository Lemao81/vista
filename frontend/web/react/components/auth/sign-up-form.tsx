import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation } from '@tanstack/react-query';
import { useContext, useEffect, useEffectEvent } from 'react';
import { type SubmitHandler, useForm } from 'react-hook-form';
import { toast } from 'sonner';
import type { z } from 'zod';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';
import Checkbox from '@/components/ui/checkbox';
import Input from '@/components/ui/input';
import { signUp } from '@/requests/auth/signUp';
import { type SignUpFormData, signUpFormDataSchema } from '@/schemas/auth';
import {
  getValidationErrors,
  isDevelopment,
  isValidationFailedError,
  jsonify,
} from '@/utils/helpers';

export default function SignUpForm() {
  const { setShowSignUpModal } = useContext(ModalContext);

  const {
    register,
    handleSubmit,
    reset,
    setError,
    watch,
    formState: { errors, isSubmitting, isSubmitSuccessful },
  } = useForm<z.input<typeof signUpFormDataSchema>, unknown, z.output<typeof signUpFormDataSchema>>(
    {
      resolver: zodResolver(signUpFormDataSchema),
      defaultValues: (isDevelopment() && getDevDefault()) || undefined,
    }
  );

  const acceptTerms = watch('acceptTerms');

  const onSuccessfulSubmit = useEffectEvent((_: boolean) => reset());
  useEffect(() => onSuccessfulSubmit(isSubmitSuccessful), [isSubmitSuccessful]);

  const { mutate } = useMutation({
    mutationFn: signUp,
    onSuccess: (_) => {
      toast.success('You have been signed up');
    },
    onError: (error) => {
      if (isValidationFailedError(error)) {
        for (const [name, errors] of getValidationErrors(error)) {
          setError(name as keyof SignUpFormData, {
            message: errors[0],
          });
        }

        return;
      }

      console.error(jsonify(error));
      toast.error(error?.message ? `Error: ${error.message}` : 'Unknown error occurred');
    },
  });

  const onSubmit: SubmitHandler<SignUpFormData> = (formData) => {
    if (!formData.acceptTerms) {
      setError('acceptTerms', {
        message: 'You must agree to our terms and conditions',
      });

      return;
    }

    if (formData.password !== formData.passwordRepeat) {
      setError('passwordRepeat', {
        type: 'manual',
        message: 'Must equal password.',
      });

      return;
    }

    console.log(`formData=${jsonify(formData)}`);

    mutate({
      userName: formData.userName,
      email: formData.email,
      password: formData.password,
      passwordRepeat: formData.passwordRepeat,
    });
  };

  function getDevDefault() {
    return {
      userName: 'test',
      email: 'test@test.com',
      password: 'Test01!',
      passwordRepeat: 'Test01!',
      acceptTerms: true,
    };
  }

  return (
    <>
      <h1 className="text-3xl font-bold mb-8">Sign Up</h1>
      <form className="grid gap-2 min-w-87.5" onSubmit={handleSubmit(onSubmit)}>
        <Input
          {...register('userName')}
          error={errors.userName?.message}
          label={'User name'}
          placeholder={'Please chose a user name'}
        />
        <Input
          {...register('email')}
          error={errors.email?.message}
          label={'Email'}
          placeholder={'Please enter your email'}
        />
        <Input
          {...register('password')}
          error={errors.password?.message}
          isPassword={true}
          label={'Password'}
          placeholder={'Your password must be at least 6 characters'}
        />
        <Input
          {...register('passwordRepeat')}
          error={errors.passwordRepeat?.message}
          isPassword={true}
          label={'Confirm Password'}
          placeholder={'Repeat your password'}
        />
        <Checkbox
          {...register('acceptTerms')}
          error={errors.acceptTerms?.message}
          label={'I accept the Terms and Conditions'}
        />
        <div className="grid grid-cols-2 gap-4">
          <Button
            className="bg-(--primary) hover:bg-(--primary-bright) active:bg-(--primary-dim)"
            disabled={!acceptTerms || isSubmitting}
            text={'OK'}
            type={'submit'}
          />
          <Button
            className="border border-neutral-300 bg-white hover:bg-gray-100 active:bg-gray-200 text-black"
            onClick={() => {
              reset();
              setShowSignUpModal(false);
            }}
            text={'Cancel'}
          />
        </div>
      </form>
    </>
  );
}
