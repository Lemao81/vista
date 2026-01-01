import Input from '@/components/ui/input';
import { useContext, useEffect } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';
import { z } from 'zod';
import { SubmitHandler, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import Checkbox from '@/components/ui/checkbox';
import { VALIDATION_MESSAGES } from '@/utils/messages';
import { signUpUser } from '@/requests/auth/signUpUser';
import { useMutation } from '@tanstack/react-query';
import { isDevelopment, jsonify } from '@/utils/helpers';

const formDataSchema = z.object({
  userName: z
    .string()
    .nonempty(VALIDATION_MESSAGES.nonEmpty)
    .regex(/^[a-zA-Z0-9]*$/, { error: 'Must only contain letters and digits.' })
    .trim(),
  email: z.string().nonempty(VALIDATION_MESSAGES.nonEmpty).email().trim(),
  password: z
    .string()
    .nonempty(VALIDATION_MESSAGES.nonEmpty)
    .min(6, { message: 'Must be at least 6 characters.' })
    .trim(),
  passwordRepeat: z.string().nonempty(VALIDATION_MESSAGES.nonEmpty).trim(),
  acceptTerms: z.boolean(),
});

type FormData = z.infer<typeof formDataSchema>;

export default function SignUpForm() {
  const { setShowSignUpModal } = useContext(ModalContext);

  const {
    register,
    handleSubmit,
    reset,
    setError,
    watch,
    formState: { errors, isSubmitting, isSubmitSuccessful },
  } = useForm<z.input<typeof formDataSchema>, any, z.output<typeof formDataSchema>>({
    resolver: zodResolver(formDataSchema),
    defaultValues: (isDevelopment() && getDevDefault()) || undefined,
  });

  const acceptTerms = watch('acceptTerms');

  useEffect(() => {
    reset();
  }, [isSubmitSuccessful]);

  const { status, mutate } = useMutation({
    mutationFn: signUpUser,
    onError: (error) => console.error(error),
    onSuccess: (data) => {
      if (data) {
        console.log(jsonify(data));
      }
    },
  });

  const onSubmit: SubmitHandler<FormData> = (formData) => {
    if (!formData.acceptTerms) {
      setError('acceptTerms', {
        type: 'manual',
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
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-col items-center w-100 px-6">
        <h1 className="text-3xl font-bold">Sign Up</h1>
        <div className="flex flex-col w-full gap-1 mt-10">
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
        </div>
        <div className="flex gap-4 mt-8">
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
      </div>
    </form>
  );
}
