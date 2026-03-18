'use client';

import { useContext } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';

export default function AuthButtonPanel() {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <div className="flex items-center gap-4 pr-8">
      <Button pulse text={'Sign In'} />
      <Button extent="lg" onClick={() => setShowSignUpModal(true)} pulse text={'Sign Up'} />
    </div>
  );
}
