import Input from '@/components/ui/input';
import { useContext, useEffect } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';
import { z } from 'zod';
import { SubmitHandler, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';

const formDataSchema = z.object({
  userName: z
    .string()
    .nonempty('Must not be empty')
    .regex(/^[a-zA-Z0-9]*$/, { error: 'Must only contain letters and digits' })
    .trim(),
  email: z.string().nonempty('Must not be empty').email().trim(),
  password: z
    .string()
    .nonempty('Must not be empty')
    .min(6, { message: 'Must be at least 6 characters' })
    .trim(),
});

type FormData = z.infer<typeof formDataSchema>;

export default function SignUpForm() {
  const { setShowSignUpModal } = useContext(ModalContext);
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting, isSubmitSuccessful },
  } = useForm<z.input<typeof formDataSchema>, any, z.output<typeof formDataSchema>>({
    resolver: zodResolver(formDataSchema),
  });

  useEffect(() => {
    reset();
  }, [isSubmitSuccessful]);

  const onSubmit: SubmitHandler<FormData> = (formData) => {
    formData && console.log(formData);
  };

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
        </div>
        <div className="flex gap-4 mt-8">
          <Button
            text={'OK'}
            type={'submit'}
            disabled={isSubmitting}
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
