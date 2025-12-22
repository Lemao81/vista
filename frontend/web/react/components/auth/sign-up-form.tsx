import Input from '@/components/ui/input';
import { useContext } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';

export default function SignUpForm() {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <div className="flex flex-col items-center w-100 px-6">
      <h1 className="text-3xl font-bold">Sign Up</h1>
      <div className="flex flex-col w-full gap-2 mt-6">
        <Input label={'User name'} placeholder={'Please chose a user name'} />
        <Input label={'Email'} placeholder={'Please enter your email'} />
        <Input
          label={'Password'}
          placeholder={'Your password must be at least 6 characters'}
          isPassword={true}
        />
      </div>
      <div className="flex gap-4 mt-10">
        <Button text={'OK'} className="bg-blue-500 hover:bg-blue-400 active:bg-blue-600" />
        <Button
          text={'Cancel'}
          className="border border-neutral-300 bg-white hover:bg-gray-100 active:bg-gray-200 text-black"
          onClick={() => setShowSignUpModal(false)}
        />
      </div>
    </div>
  );
}
