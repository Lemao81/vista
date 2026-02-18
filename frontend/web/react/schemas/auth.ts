import { VALIDATION_MESSAGES } from '@/utils/messages';
import { z } from 'zod';

export const signUpFormDataSchema = z.object({
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

export type SignUpFormData = z.infer<typeof signUpFormDataSchema>;
