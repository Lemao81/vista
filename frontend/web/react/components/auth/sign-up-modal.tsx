import Modal from '@/components/shared/modal';
import SignUpForm from '@/components/auth/sign-up-form';
import { useContext } from 'react';
import { ModalContext } from '@/components/shared/modal-provider';

export default function SignUpModal({ show }: { show: boolean }) {
  const { setShowSignUpModal } = useContext(ModalContext);

  return (
    <Modal show={show} onDismiss={() => setShowSignUpModal(false)}>
      <SignUpForm />
    </Modal>
  );
}
