'use client';

import { useContext } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';
import Button from '@/components/ui/button';

export default function AuthButtonPanel() {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <div className="flex items-center gap-4 pr-8">
      <Button
        className="bg-brand hover:bg-(--brand-bright) active:bg-(--brand-dim) hover:scale-105"
        text={'Sign In'}
      />
      <Button
        className="rounded-md bg-brand px-5 py-2 hover:bg-(--brand-bright) active:bg-(--brand-dim) hover:scale-105"
        onClick={() => setShowSignUpModal(true)}
        text={'Sign Up'}
      />
    </div>
  );
}
