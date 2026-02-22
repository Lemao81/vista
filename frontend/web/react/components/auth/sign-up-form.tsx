import Input from '@/components/ui/input';
import { useContext, useEffect } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';
import { z } from 'zod';
import { SubmitHandler, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import Checkbox from '@/components/ui/checkbox';
import { signUp } from '@/requests/auth/signUp';
import { useMutation } from '@tanstack/react-query';
import {
  getValidationErrors,
  isDevelopment,
  isValidationFailedError,
  jsonify,
} from '@/utils/helpers';
import { toast } from 'sonner';
import { SignUpFormData, signUpFormDataSchema } from '@/schemas/auth';

export default function SignUpForm() {
  const { setShowSignUpModal } = useContext(ModalContext);

  const {
    register,
    handleSubmit,
    reset,
    setError,
    watch,
    formState: { errors, isSubmitting, isSubmitSuccessful },
  } = useForm<z.input<typeof signUpFormDataSchema>, any, z.output<typeof signUpFormDataSchema>>({
    resolver: zodResolver(signUpFormDataSchema),
    defaultValues: (isDevelopment() && getDevDefault()) || undefined,
  });

  const acceptTerms = watch('acceptTerms');

  useEffect(() => {
    reset();
  }, [isSubmitSuccessful]);

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
      <form onSubmit={handleSubmit(onSubmit)} className="grid gap-2 min-w-87.5">
          <Input
            {...register('userName')}
            label={'User name'}
            placeholder={'Please chose a user name'}
            error={errors.userName?.message}
          />
          <Input
            {...register('email')}
            label={'Email'}
            placeholder={'Please enter your email'}
            error={errors.email?.message}
          />
          <Input
            {...register('password')}
            label={'Password'}
            placeholder={'Your password must be at least 6 characters'}
            isPassword={true}
            error={errors.password?.message}
          />
          <Input
            {...register('passwordRepeat')}
            label={'Confirm Password'}
            placeholder={'Repeat your password'}
            isPassword={true}
            error={errors.passwordRepeat?.message}
          />
          <Checkbox
            {...register('acceptTerms')}
            label={'I accept the Terms and Conditions'}
            error={errors.acceptTerms?.message}
          />
        <div className="grid grid-cols-2 gap-4">
          <Button
            text={'OK'}
            type={'submit'}
            disabled={!acceptTerms || isSubmitting}
            className="bg-blue-500 hover:bg-blue-400 active:bg-blue-600"
          />
          <Button
            text={'Cancel'}
            className="border border-neutral-300 bg-white hover:bg-gray-100 active:bg-gray-200 text-black"
            onClick={() => {
              reset();
              setShowSignUpModal(false);
            }}
          />
        </div>
      </form>
    </>
  );
}
